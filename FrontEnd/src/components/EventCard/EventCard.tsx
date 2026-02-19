import { ICONS } from '../../config/icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import dateTimeToString from '../../utils/dateTimeToString';
import {useState,useEffect} from 'react';
import { Link } from 'react-router-dom';
import styles from './EventCard.module.css';

interface Address {
  country: string;
  countryCode: string;
  city: string;
  venue: string;
}

type EventCardProps = {
    id: string;
    imageUrl: string;
    price: number;
    title: string;
    datetime: string;
    address: Address;
    index: number;
}

function EventCard({ id, imageUrl, price, title, datetime, address, index }: EventCardProps) {
    const [isLoaded, setIsLoaded] = useState(false);
    useEffect(() => {
        setIsLoaded(true);
    }, []);
    const animationStyle = {
        animationDelay: `${index * 0.1}s`
    };

  const formattedDate = dateTimeToString(datetime);
  return(
    <div className={`${styles.Card} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`} 
        style={animationStyle}>
        <div className={styles.CardImage}> 
            <img 
                src={imageUrl || "https://placehold.co/600x400?text=No+Image"} 
                alt="Event" 
            />
        </div>
        <div className={styles.CardInfoBox}>
            <div className={styles.CardInfo}> 
                <h3>{title}</h3>  
                  <div className={styles.Miscellaneous}>
                    <div className={styles.infoRow}>
                      <FontAwesomeIcon icon={ICONS.calendar} />
                      <div className={styles.DateBox}>
                        <p>{formattedDate.monthName} {formattedDate.dayNumber}, {formattedDate.time}</p>
                      </div>
                    </div>

                    <div className={styles.infoRow}>
                      <FontAwesomeIcon icon={ICONS.dollar} />
                      <p>{price > 0 ? `$${price.toFixed(2)}` : 'Free'}</p>
                    </div>

                    <div className={styles.infoRow}>
                      <FontAwesomeIcon icon={ICONS.globe} />
                      <p>{address.country}, {address.city}</p>
                    </div>
                  </div>
            </div>
          <Link className={styles.BookButton} to={`/events/${id}`}>
            Book Now
          </Link>
        </div>
    </div>
  )
}

export default EventCard;