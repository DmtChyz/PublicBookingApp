import styles from './ContactsPage.module.css';
import { ICONS } from '../../config/icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {useState,useEffect} from 'react';
import { useNotification } from '../../context/NotificationContext/NotificationContext';

function ContactsPage(){
    const { showNotification } = useNotification();

    const contactData = [
    {
        type: 'ContactItem',
        icon: ICONS.envelope,
        title: 'General Inquiries',
        copyText: 'support@bookingapp.com',
        copySuccessMessage: 'Email successfully copied!'
    },
    {
        type: 'ContactItem',
        icon: ICONS.phone,
        title: 'Phone Support',
        copyText: '+1 (555) 123-4567',
        copySuccessMessage: 'Phone successfully copied!'
    },
    {
        type: 'OfficeItem',
        icon: ICONS.globe,
        title: 'Our Office',
        text: '123 Event Lane, New York, NY, USA'
    },
    {
        type: 'HoursItem',
        icon: ICONS.clock,
        title: 'Business Hours',
        text: 'Monday - Friday: 9:00 AM to 5:00 PM EST',
        subtext: 'Closed on weekends and public holidays.'
    }
    ];
    const [isLoaded, setIsLoaded] = useState(false);
    
      useEffect(() => {
        setIsLoaded(true);
      }, []);

    const handleCopyClick = (textToCopy: string, successMessage: string) => {
        navigator.clipboard.writeText(textToCopy);
        showNotification(successMessage, 'success');
    };
    return(
    <div className={styles.contactContainer}>
            <header className={styles.header}>
                <h3>Get In Touch</h3>
                <h2>We're here to help. Reach out to us through any of the channels below.</h2>
            </header>

            <main className={styles.contactCard}>
                {contactData.map((item, index) => {                
                    switch (item.type) {
            
                        case 'ContactItem':
                        if (item.copyText && item.copySuccessMessage) {
                            return (
                            <div
                                key={item.title}
                                className={`${styles.contactItem} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}
                                style={{ animationDelay: `${index * 0.2}s` }}
                            >
                                <div className={styles.icon}>
                                    <FontAwesomeIcon icon={item.icon} />
                                </div>
                                <div className={styles.itemDetails}>
                                <h3>{item.title}</h3>
                                <div 
                                    className={styles.CopyBox}
                                    onClick={() => handleCopyClick(item.copyText, item.copySuccessMessage)}
                                >
                                    <span><a>{item.copyText}</a></span>
                                    <FontAwesomeIcon icon={ICONS.copy} className={styles.copyIcon} />
                                </div>
                                </div>
                            </div>
                            );
                        }
                        return null;
                        
                        case 'OfficeItem':
                        return (
                            <div
                            key={item.title}
                            className={`${styles.contactItem} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}
                            style={{ animationDelay: `${index * 0.2}s` }}
                            >
                                <div className={styles.icon}>
                                    <FontAwesomeIcon icon={item.icon} />
                                </div>
                                <div className={styles.itemDetails}>
                                    <h3>{item.title}</h3>
                                    <p>{item.text}</p>
                                </div>
                            </div>
                        );
                        case 'HoursItem':
                        if (item.text && item.subtext) {
                            return (
                            <div
                                key={item.title}
                                className={`${styles.businessHoursItem} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}
                                style={{ animationDelay: `${index * 0.2}s` }}
                            >
                                <div className={styles.icon}>
                                    <FontAwesomeIcon icon={item.icon} />
                                </div>
                                <div className={styles.itemDetails}>
                                <h3>{item.title}</h3>
                                <p>{item.text}</p>
                                <span className={styles.subtext}>{item.subtext}</span>
                                </div>
                            </div>
                            );
                        }
                        return null;
                        default:
                            return null;
                }

                })}
            </main>
        </div>
    )
}
export default ContactsPage;