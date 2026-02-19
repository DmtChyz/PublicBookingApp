import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useNotification } from '../../context/NotificationContext/NotificationContext';
import { createEvent,generateEventDescription, type CreateEventData,type GenerateDescriptionData } from '../../api/events/events';
import type { Event } from '../../types/Event';
import styles from './CreateEventPage.module.css';
import { format, addDays } from 'date-fns';

const defaultDate = format(addDays(new Date(), 1), "yyyy-MM-dd'T'12:00");

const initialState: CreateEventData = {
    title: '',
    description: '',
    eventDate: defaultDate,
    maxAttendees: 10,
    imageUrl: '',
    price: 0,
    address: {
        country: '',
        countryCode: '',
        city: '',
        venue: ''
    }
};

function CreateEventPage() {
    const navigate = useNavigate();
    const queryClient = useQueryClient();
    const { showNotification } = useNotification();
    const [formData, setFormData] = useState<CreateEventData>(initialState);
    const [isVisible, setIsVisible] = useState(false);
    const [aiPrompt, setAiPrompt] = useState('');

    useEffect(() => {
        setIsVisible(true);
    }, []);

    const { mutate, isPending, error } = useMutation({
        mutationFn: createEvent,
        onSuccess: (data: Event) => {
            queryClient.invalidateQueries({ queryKey: ['events'] });
            showNotification('Event successfully created', 'success');
            navigate(`/events/${data.id}`);
        },
        onError: (err: Error) => {
            console.error("Failed to create event:", err);
        }
    });
    const { mutate: generateDescription, isPending: isGenerating } = useMutation({
        mutationFn: generateEventDescription,
        onSuccess: (generatedDescription: string) => {
            setFormData(prev => ({ ...prev, description: generatedDescription }));
            showNotification('Description generated successfully!', 'success');
        },
        onError: (err: Error) => {
            showNotification(err.message, 'error');
        }
    });

    const handleGenerateDescription = () => {
        if (aiPrompt.trim() === '') {
            showNotification('Please tell us something about your event.', 'error');
            return;
        }
        const dataForAI: GenerateDescriptionData = {
            title: formData.title,
            city: formData.address.city,
            country: formData.address.country,
            price: formData.price,
            userPrompt: aiPrompt
        };
        generateDescription(dataForAI);
    };


    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value, type } = e.target;
        const finalValue = type === 'number' ? Number(value) : value;
        setFormData(prev => ({ ...prev, [name]: finalValue }));
    };

    const handleAddressChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({...prev, address: { ...prev.address, [name]: value }}));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        const eventDataToSend = {
            ...formData,
            eventDate: new Date(formData.eventDate).toISOString()
        };
        mutate(eventDataToSend);
    };

    return (
        <div className={styles.pageWrapper}>
            <div className={`${styles.formCard} ${styles.fadeInOnLoad} ${isVisible ? styles.visible : ''}`}>
                <h2 className={styles.title}>Create New Event</h2>
                <p className={styles.subtitle}>Fill in the details to host your event</p>
                
                <form onSubmit={handleSubmit} className={styles.formGrid} noValidate>
                    {error && <div className={styles.errorMessage}>{error.message}</div>}
                    
                    <div className={`${styles.fieldBlock} ${styles.fullWidth}`}>
                        <label htmlFor="title">Title</label>
                        <input id="title" name="title" type="text" value={formData.title} onChange={handleInputChange} placeholder='Event name'
                               required minLength={8} maxLength={80} pattern="^[\p{L}\p{N}_ '.-]+$" />
                    </div>
                    <div className={`${styles.fieldBlock} ${styles.fullWidth}`}>
                        <label htmlFor="description">Description</label>
                        <textarea id="description" name="description" value={formData.description} onChange={handleInputChange} rows={4} placeholder='Generate description using AI. The button is bellow.'
                                  required minLength={24} maxLength={512} />
                    </div>

                    <div className={styles.fieldBlock}>
                        <label htmlFor="eventDate">Date and Time</label>
                        <input id="eventDate" name="eventDate" type="datetime-local" value={formData.eventDate} onChange={handleInputChange} 
                               required />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="imageUrl">Image URL</label>
                        <input id="imageUrl" name="imageUrl" type="url" value={formData.imageUrl} onChange={handleInputChange} 
                               required minLength={20} />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="maxAttendees">Max Attendees</label>
                        <input id="maxAttendees" name="maxAttendees" type="number" min="1" value={formData.maxAttendees} onChange={handleInputChange} 
                               required />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="price">Price in $ Per Ticket</label>
                        <input id="price" name="price" type="number" min="0" step="0.01" value={formData.price} onChange={handleInputChange} 
                               required />
                    </div>

                    <h3 className={styles.addressTitle}>Location</h3>

                    <div className={styles.fieldBlock}>
                        <label htmlFor="country">Country</label>
                        <input id="country" name="country" type="text" value={formData.address.country} onChange={handleAddressChange} 
                               required maxLength={60} />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="countryCode">Country Code</label>
                        <input id="countryCode" name="countryCode" type="text" value={formData.address.countryCode} onChange={handleAddressChange} 
                               required minLength={2} maxLength={2} />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="city">City</label>
                        <input id="city" name="city" type="text" value={formData.address.city} onChange={handleAddressChange} 
                               required maxLength={85} />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label htmlFor="venue">Venue</label>
                        <input id="venue" name="venue" type="text" value={formData.address.venue} onChange={handleAddressChange} 
                               required maxLength={100} />
                    </div>
                    
                    <button type="submit" className={styles.submitBtn} disabled={isPending}>
                        {isPending ? 'Creating...' : 'Create Event'}
                    </button>
                </form>
                <form className={styles.formGrid} onSubmit={(e) => e.preventDefault()}>
                    <div className={styles.AiBlock}>
                        <div className={`${styles.fieldBlock} ${styles.fullWidth}`}>
                            <label htmlFor="aiPrompt">Tell us about your event!</label>
                            <textarea 
                                id="aiPrompt" 
                                name="aiPrompt" 
                                value={aiPrompt} 
                                onChange={(e) => setAiPrompt(e.target.value)} 
                                rows={4} 
                                placeholder='Tell the AI what kind of vibe you want for the description!' 
                            />
                        </div>
                        <button 
                            type="button" 
                            className={styles.submitBtn} 
                            onClick={handleGenerateDescription}
                            disabled={isGenerating || aiPrompt.trim() === ''}
                        >
                            {isGenerating ? 'Generating...' : 'Generate Description with AI'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default CreateEventPage;