import { useState, useEffect } from 'react';
import styles from './BookingList.module.css';
import { useAuth } from '../../context/AuthContext/AuthContext';
import { useNotification } from '../../context/NotificationContext/NotificationContext';
import { getMyBookings, deleteBooking } from '../../api/bookings/bookings';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';

function BookingList() {
    const { token } = useAuth();
    const { showNotification } = useNotification();
    const queryClient = useQueryClient();
    const [isVisible, setIsVisible] = useState(false);

    useEffect(() => {
        setIsVisible(true);
    }, []);

    const { data: bookings = [], isLoading, error } = useQuery({
        queryKey: ['userBookings'],
        queryFn: () => getMyBookings(),
        enabled: !!token,
    });

    const cancelBookingMutation = useMutation({
        mutationFn: (bookingId: number) => deleteBooking(bookingId),
        onSuccess: () => {
            showNotification("Booking cancelled successfully", "success");
            queryClient.invalidateQueries({ queryKey: ['userBookings'] });
        },
        onError: (err: Error) => {
            showNotification(err.message || "Failed to cancel booking", "error");
        }
    });

    if (isLoading) {
        return (
            <div className={styles.bookingsSection}>
                <h2 className={styles.bookingsTitle}>My Bookings</h2>
                <p>Loading...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className={styles.bookingsSection}>
                <h2 className={styles.bookingsTitle}>My Bookings</h2>
                <p className={styles.error}>Could not load your bookings.</p>
            </div>
        );
    }

    return (
        <div className={`${styles.bookingsSection} ${styles.fadeInOnLoad} ${isVisible ? styles.visible : ''}`}>
            <h2 className={styles.bookingsTitle}>My Bookings</h2>
            
            {bookings.length === 0 ? (
                <div className={styles.noBookings}>You haven't booked any events yet.</div>
            ) : (
                <div className={styles.bookingGrid}>
                    {bookings.map((booking) => (
                        <div key={booking.id} className={styles.bookingCard}>
                            <div style={{display:'flex', justifyContent:'space-between', alignItems:'center'}}>
                                <h3 className={styles.eventTitle}>{booking.eventSummary.title}</h3>
                                <span style={{color:'#666', fontSize:'0.9rem'}}>#{booking.id}</span>
                            </div>
                            
                            <div className={styles.bookingDetail}>
                                <span>Event Date:</span> {new Date(booking.eventSummary.eventDate).toLocaleDateString()}
                            </div>
                            <div className={styles.bookingDetail}>
                                <span>Booked On:</span> {new Date(booking.createdAt).toLocaleDateString()}
                            </div>
                            <div className={styles.bookingDetail}>
                                <span>Seats:</span> {booking.numberOfSeats}
                            </div>
                            <div className={styles.bookingDetail}>
                                <span>Total Price:</span> ${booking.eventSummary.price * booking.numberOfSeats}
                            </div>
                            <div className={styles.bookingDetail}>
                                <span>Status:</span> {booking.status}
                            </div>
                            
                            {booking.notes && (
                                <div className={styles.bookingDetail}>
                                    <span>Notes:</span> <i style={{color:'#CCC'}}>{booking.notes}</i>
                                </div>
                            )}

                            <button 
                                className={styles.cancelBookingBtn}
                                onClick={() => cancelBookingMutation.mutate(booking.id)}
                                disabled={cancelBookingMutation.isPending}
                            >
                                {cancelBookingMutation.isPending ? "Cancelling..." : "Cancel Booking"}
                            </button>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default BookingList;