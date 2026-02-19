import styles from './Notification.module.css';
import { ICONS } from '../../config/icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, useEffect } from 'react';

interface NotificationProps{
    message: string;
    type: 'success' | 'error';
    onExit: () => void;
};

function Notification(props: NotificationProps){

    const [animate, setAnimate] = useState(false);
    const [isFilling, setIsFilling] = useState(false);
    const [isExiting,setExiting] = useState(false);

    useEffect(() => {
        const borderTimer = setTimeout(() => {
            setAnimate(true);
        }, 10);

        const fillTimer = setTimeout(() => {
            setIsFilling(true);
        }, 550);
        const exitTimer = setTimeout(() =>{
            setExiting(true)
            setTimeout(props.onExit,400);
        }, 3600)

        return () => {
            clearTimeout(borderTimer);
            clearTimeout(fillTimer);
            clearTimeout(exitTimer);
        };
    }, []);

    const notificationClasses = `
        ${styles.Notification}
        ${animate ? styles.animateBorder : ''}
        ${isExiting ? styles.exiting : ''}
    `;
    const notificationBoxClasses = `
        ${styles.notificationBox} 
        ${isFilling ? styles.fillContent : ''}
        ${props.type === 'success' ? styles.successFill : styles.errorFill}
    `;
    
    return(
        <div className={notificationClasses}>
            <div className={notificationBoxClasses}>
                <FontAwesomeIcon icon={ICONS.circle} className={styles.icon} />
            </div>
            
            <div className={styles.line}></div>
            
            <div className={styles.MessageBox}>
                <p>{props.message}</p>
            </div>
        </div>
    )
}

export default Notification;