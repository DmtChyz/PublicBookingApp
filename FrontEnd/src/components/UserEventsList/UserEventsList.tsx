import { useState, useEffect } from 'react';
import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { getMyEvents } from '../../api/events/events';
import styles from './UserEventsList.module.css';

function UserEventsList() {
    const [isVisible, setIsVisible] = useState(false);

    useEffect(() => {
        setIsVisible(true);
    }, []);
    
    const { data: events = [], isLoading, isError, error, refetch } = useQuery({
        queryKey: ['myEvents'],
        queryFn: getMyEvents
    });

    if (isLoading) {
        return (
            <div className={styles.eventsSection}>
                <h2 className={styles.eventsTitle}>My Created Events</h2>
                <p>Loading your events...</p>
            </div>
        );
    }

    if (isError) {
        return (
            <div className={styles.eventsSection}>
                <h2 className={styles.eventsTitle}>My Created Events</h2>
                <div className={styles.errorContainer}>
                    <h3>Oops! Something went wrong.</h3>
                    <p>{error.message}</p>
                    <button onClick={() => refetch()} className={styles.actionBtn}>Try Again</button>
                </div>
            </div>
        );
    }

    return (
        <div className={`${styles.eventsSection} ${styles.fadeInOnLoad} ${isVisible ? styles.visible : ''}`}>
            <h2 className={styles.eventsTitle}>My Created Events</h2>
            
            {events.length === 0 ? (
                <div className={styles.noEvents}>
                    You haven't created any events yet. <Link to="/createEvent">Create one now!</Link>
                </div>
            ) : (
                <div className={styles.eventGrid}>
                    {events.map((event) => (
                        <div key={event.id} className={styles.eventCard}>
                            <img src={event.imageUrl} alt={event.title} className={styles.eventImage} />
                            <div className={styles.eventContent}>
                                <h3 className={styles.eventTitle}>{event.title}</h3>
                                <div className={styles.eventDetail}>
                                    <span>Date:</span> {new Date(event.eventDate).toLocaleString()}
                                </div>
                                <div className={styles.eventDetail}>
                                    <span>Price:</span> ${event.price.toFixed(2)}
                                </div>
                                <div className={styles.eventDetail}>
                                    <span>Location:</span> {`${event.address.venue}, ${event.address.city}`}
                                </div>
                                <div className={styles.cardActions}>
                                    <Link to={`/events/${event.id}/edit`} className={styles.actionBtn}>
                                        Edit
                                    </Link>
                                    <Link to={`/events/${event.id}`} className={`${styles.actionBtn} ${styles.viewBtn}`}>
                                        View
                                    </Link>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default UserEventsList;