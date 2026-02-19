import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useNotification } from '../../context/NotificationContext/NotificationContext';
import { getEventById, updateEvent, deleteEvent, type UpdateEventData } from '../../api/events/events';
import type { Event } from '../../types/Event';
import LoadingScreen from '../../components/LoadingScreen/LoadingScreen';
import styles from './EditEventPage.module.css';

function EditEventPage() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const queryClient = useQueryClient();
    const { showNotification } = useNotification();
    
    const [formData, setFormData] = useState<Event | null>(null);
    const [isVisible, setIsVisible] = useState(false);
    const [isConfirmingDelete, setIsConfirmingDelete] = useState(false);

    useEffect(() => { setIsVisible(true); }, []);

    const { data: originalEvent, isLoading, isError, error } = useQuery({
        queryKey: ['event', id],
        queryFn: () => getEventById(id!),
        enabled: !!id,
    });

    useEffect(() => {
        if (originalEvent) {
            const localDate = new Date(originalEvent.eventDate);
            const formattedDate = new Date(localDate.getTime() - (localDate.getTimezoneOffset() * 60000)).toISOString().slice(0, 16);
            setFormData({ ...originalEvent, eventDate: formattedDate });
        }
    }, [originalEvent]);

    const { mutate: performUpdate, isPending: isUpdating } = useMutation({
        mutationFn: (dataToUpdate: UpdateEventData) => updateEvent({ eventId: Number(id!), eventData: dataToUpdate }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['events', 'myEvents', ['event', id]] });
            showNotification('Event updated successfully!', 'success');
            navigate(`/events/${id}`);
        },
        onError: (err: Error) => {
            showNotification(err.message || "Failed to update event", "error");
        }
    });

    const { mutate: performDelete, isPending: isDeleting } = useMutation({
        mutationFn: deleteEvent,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ['events', 'myEvents'] });
            showNotification('Event deleted successfully', 'success');
            navigate('/profile');
        },
        onError: (err: Error) => {
            showNotification(err.message || 'Failed to delete event', 'error');
        }
    });

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        if (!formData) return;
        const { name, value, type } = e.target;
        const finalValue = type === 'number' ? Number(value) : value;
        setFormData(prev => ({ ...prev!, [name]: finalValue }));
    };

    const handleAddressChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (!formData) return;
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev!, address: { ...prev!.address, [name]: value } }));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!formData) return;
        const dataToSubmit: UpdateEventData = {
            title: formData.title,
            description: formData.description,
            eventDate: new Date(formData.eventDate).toISOString(),
            maxAttendees: formData.maxAttendees,
            imageUrl: formData.imageUrl,
            address: formData.address,
        };
        performUpdate(dataToSubmit);
    };

    const handleConfirmDelete = () => {
        if (id) {
            performDelete(Number(id));
        }
    };

    const isActionPending = isUpdating || isDeleting;

    if (isLoading || !formData) { return <LoadingScreen />; }
    if (isError) { return <div className={styles.pageWrapper}><div className={styles.errorCard}>{error.message}</div></div>; }

    return (
        <div className={styles.pageWrapper}>
            <div className={`${styles.formCard} ${styles.fadeInOnLoad} ${isVisible ? styles.visible : ''}`}>
                <h2 className={styles.title}>Edit Event</h2>
                <p className={styles.subtitle}>Update the details for your event</p>
                
                <form onSubmit={handleSubmit} className={styles.formGrid} noValidate>
                    <div className={`${styles.fieldBlock} ${styles.fullWidth}`}>
                        <label htmlFor="title">Title</label>
                        <input id="title" name="title" type="text" value={formData.title} onChange={handleInputChange} required minLength={8} maxLength={80} />
                    </div>
                    <div className={`${styles.fieldBlock} ${styles.fullWidth}`}>
                        <label htmlFor="description">Description</label>
                        <textarea id="description" name="description" value={formData.description} onChange={handleInputChange} rows={4} required minLength={24} maxLength={512} />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="eventDate">Date and Time</label>
                        <input id="eventDate" name="eventDate" type="datetime-local" value={formData.eventDate} onChange={handleInputChange} required />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="imageUrl">Image URL</label>
                        <input id="imageUrl" name="imageUrl" type="url" value={formData.imageUrl} onChange={handleInputChange} required minLength={20} />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="maxAttendees">Max Attendees</label>
                        <input id="maxAttendees" name="maxAttendees" type="number" min="1" value={formData.maxAttendees} onChange={handleInputChange} required />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="price">Price (Cannot be changed)</label>
                        <div className={styles.readOnlyField}>${formData.price.toFixed(2)}</div>
                    </div>
                    <h3 className={styles.addressTitle}>Location</h3>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="country">Country</label>
                        <input id="country" name="country" type="text" value={formData.address.country} onChange={handleAddressChange} required maxLength={60} />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="countryCode">Country Code</label>
                        <input id="countryCode" name="countryCode" type="text" value={formData.address.countryCode} onChange={handleAddressChange} required minLength={2} maxLength={2} />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="city">City</label>
                        <input id="city" name="city" type="text" value={formData.address.city} onChange={handleAddressChange} required maxLength={85} />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="venue">Venue</label>
                        <input id="venue" name="venue" type="text" value={formData.address.venue} onChange={handleAddressChange} required maxLength={100} />
                    </div>
                    
                    <div className={styles.buttonGroup}>
                        {!isConfirmingDelete ? (
                            <button type="button" className={`${styles.formBtn} ${styles.deleteBtn}`} onClick={() => setIsConfirmingDelete(true)} disabled={isActionPending}>
                                Delete Event
                            </button>
                        ) : (
                            <div className={styles.confirmGroup}>
                                <button type="button" className={`${styles.formBtn} ${styles.confirmCancelBtn}`} onClick={() => setIsConfirmingDelete(false)} disabled={isActionPending}>
                                    Cancel
                                </button>
                                <button type="button" className={`${styles.formBtn} ${styles.confirmDeleteBtn}`} onClick={handleConfirmDelete} disabled={isActionPending}>
                                    {isDeleting ? 'Deleting...' : 'Confirm Delete'}
                                </button>
                            </div>
                        )}
                        <button type="submit" className={`${styles.formBtn} ${styles.submitBtn}`} disabled={isActionPending}>
                            {isUpdating ? 'Saving...' : 'Save Changes'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default EditEventPage;