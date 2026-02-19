import { useParams, useNavigate } from "react-router-dom";
import { getEventById } from "../../api/events/events";
import { createBooking, type CreateBookingRequest } from "../../api/bookings/bookings";
import { useState } from 'react';
import styles from './EventDetailsPage.module.css';
import LoadingScreen from "../../components/LoadingScreen/LoadingScreen";
import { ICONS } from '../../config/icons';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import dateTimeToString from '../../utils/dateTimeToString';
import { useAuth } from "../../context/AuthContext/AuthContext";
import { useNotification } from "../../context/NotificationContext/NotificationContext";
import { useQuery, useMutation } from '@tanstack/react-query';

function EventDetailsPage(){
    
    const params = useParams();
    const eventId = params.eventId;
    const {data:event,isLoading:isEventLoading,error} = useQuery({
        queryKey:[`eventDetails`,eventId],
        queryFn: () => getEventById(eventId!),
        enabled: !!eventId,
        // dont run in case eventId is empty   
    })
    
    const { token } = useAuth();
    const { showNotification } = useNotification();
    const navigate = useNavigate();

    const [showTooltip, setShowTooltip] = useState(false);
    
    const formattedDate = event ? dateTimeToString(event.eventDate) : null;

    const [numberOfSeats, setNumberOfSeats] = useState<number>(1);
    const [notes, setNotes] = useState<string>("");

    const { mutate: bookEvent, isPending: isBookingLoading } = useMutation({
        mutationFn: (bookingData: CreateBookingRequest) =>{
            return createBooking(bookingData);
        },
        onSuccess: () =>{
            showNotification("Booking successful!", "success");
            navigate("/profile");
        },
        onError: (err: Error) =>{
            const errorMessage = err.message || "Failed to book event.";
            showNotification(errorMessage, "error");
        }
        });

    const handleBookingSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (!token) {
        navigate('/login');
        return;
        }
        if (numberOfSeats < 1) {
            showNotification("You need to book at least 1 seat.", "error");
            return;
        }
        bookEvent(
        {
            eventId: Number(eventId),
            numberOfSeats: numberOfSeats, 
            notes: notes                 
         }
        );
    };

    if (isEventLoading) {
        return <LoadingScreen />;
    }

    if (error) {
        return (
            <div className={styles.errorMessage}>
                <h2>Oops! Something went wrong.</h2>
                <p>{error.message}</p>
            </div>
        );
    }
    
    return (
        <div className={styles.pageContainer}>
            <div className={styles.imgBox}>
                <img src={event?.imageUrl} alt={event?.title}></img>
            </div>
            <div className={styles.detailsBox}>
                <div className={styles.TopSection}>
                    <h2>{event?.title}</h2>
                    <p>{event?.description}</p>
                </div>
                <div className={styles.backSection}>
                    <div className={styles.miscBox}>
                        <div className={styles.infoLine}>
                            <div className={styles.iconWrapper}>
                                <FontAwesomeIcon icon={ICONS.calendar}/>
                            </div>
                            Date & Time: 
                            <span className={styles.dataValue}>
                                {formattedDate ? `${formattedDate.monthName} ${formattedDate.dayNumber}, ${formattedDate.time}` : 'Loading...'}
                            </span>
                        </div>
                        <div className={styles.infoLine}>
                            <div className={styles.iconWrapper}>
                                <FontAwesomeIcon icon={ICONS.dollar}/>
                            </div>
                            Price: <span className={styles.dataValue}>${event?.price}</span> 
                            <div className={styles.DetailsIcon}
                                onMouseEnter={() => setShowTooltip(true)}
                                onMouseLeave={() => setShowTooltip(false)}
                            >
                                <FontAwesomeIcon className={styles.QuestionIcon} icon={ICONS.faQuestion}/>
                            </div>
                            {showTooltip && (
                                <div className={styles.tooltipText}>
                                    Price excludes tax & fees
                                </div>
                            )}
                        </div>
                        <div className={styles.infoLine}>
                            <div className={styles.iconWrapper}>
                                <FontAwesomeIcon icon={ICONS.globe}/>
                            </div>
                            Venue: <span className={styles.dataValue}>
                                {`${event?.address?.venue}, ${event?.address?.city}, ${event?.address?.country}`}
                            </span>
                        </div>
                    </div>
                </div>
                <div className={styles.bookingSection}>
                    <form onSubmit={handleBookingSubmit} className={styles.bookingForm}>
                        
                        <div className={styles.formSection}>
                            <div className={styles.iconWrapper}>
                                <FontAwesomeIcon icon={ICONS.faTicket}/> 
                            </div>
                            
                            <label htmlFor="numberOfSeats">Number of Seats:</label>
                            <input
                                id="numberOfSeats"
                                type="number"
                                min="1"
                                value={numberOfSeats}
                                onChange={(e) => setNumberOfSeats(parseInt(e.target.value))}
                                required
                            />
                        </div>

                        <div className={styles.formSection}>
                            <div className={styles.iconWrapper}>
                                <FontAwesomeIcon icon={ICONS.faSticky}/>
                            </div>

                            <label htmlFor="notes">Notes (Optional):</label>
                            <textarea
                                id="notes"
                                value={notes}
                                onChange={(e) => setNotes(e.target.value)}
                                rows={3}
                            />
                        </div>
                        <button type="submit" disabled={isBookingLoading}>
                            {isBookingLoading ? "Processing..." : "Confirm Booking"}
                        </button>
                    </form>
                </div>
            </div>
        </div>
    );
}
export default EventDetailsPage;