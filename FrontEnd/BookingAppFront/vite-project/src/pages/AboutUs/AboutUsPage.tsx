import { Link } from 'react-router-dom';
import styles from './AboutUsPage.module.css';
import { ICONS } from '../../config/icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useState, useEffect } from 'react';

function AboutUsPage() {
  const aboutUsData = [
    {
      type:'Hero',
      title:'Connecting You to Unforgettable Experiences',
      subtitle:'We specialize in bringing you the best live events, from local concerts to global festivals. Your next great memory is just a click away.'
    },
    {
      type:'SingleCard',
      imageSrc:'/photo/LiveEvent.jpg',
      imageAlt:'A concert with blue stage lights',
      title:'Our Mission',
      subtitle:'In a world full of digital noise, we believe in the power of live events to bring people together. Our goal is to make discovering and booking tickets seamless and exciting. We curate a diverse list of events, ensuring there is always something new for you to experience, share, and enjoy.'
    },
    {
      type:'FeatureSection',
      title:'Why should you choose us?',
      cards:[
          { icon: ICONS.faListCheck, title:'Curated Selection', subtitle:"We hand-pick the most exciting events, so you don't have to spend hours searching."},
          { icon: ICONS.faUserClock,  title:'Simple Booking', subtitle:"Our booking process is fast, secure, and easy to use. Get your tickets in just a few clicks."},
          { icon: ICONS.faUserGroup, title:'Community Focused', subtitle:"Join a community of event-lovers and share your experiences with friends."}
      ]
    },
    {
      type:'CallToAction',
      title:'Want to try our site? Then what are you waiting for?',
      buttonText:'Browse for events',
      buttonLink:'/events'
    }
  ];
  const [isLoaded, setIsLoaded] = useState(false);

  useEffect(() => {
    setIsLoaded(true);
  }, []);

  return (
    <div className={styles.AboutUsBox}>
      {aboutUsData.map((section, index) => {
        switch (section.type) {
          case 'Hero':
            return (
              <section 
                key={section.type}
                className={`${styles.HeroSection} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}
                style={{ animationDelay: `${index * 0.2}s` }}
              >
                <h3>{section.title}</h3>
                <h2>{section.subtitle}</h2>
              </section>
            );
          case 'SingleCard':
           return(
            <section
              key={section.type}
              className={`${styles.CardBox} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}
              style={{ animationDelay: `${index * 0.2}s` }}
              >
                <div className={styles.SingleCard}> 
                  <div className={styles.PhotoContainer}>
                    <img src={section.imageSrc} alt={section.imageAlt}></img>
                  </div>
                  <div className={`${styles.InfoBox}`}>
                    <h3>{section.title}</h3>
                    <p>{section.subtitle}</p>
                  </div>
                </div>
            </section>
            );
          case 'FeatureSection':
          if (!section.cards) {
            return null; 
          }
          return (
            <section
              key={section.type}
              className={`${styles.FeatureSection} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}
              style={{ animationDelay: `${index * 0.2}s` }}
            >
              <h3>{section.title}</h3>

              <div className={styles.CardContainer}>
                {section.cards.map(card => (
                  <div 
                    key={card.title}
                    className={styles.AboutUsCard}
                  >
                    <FontAwesomeIcon icon={card.icon} className={styles.Icon} />
                    <h3>{card.title}</h3>
                    <p>{card.subtitle}</p>
                  </div>
                ))}
                
              </div>
            </section>
          );
          case'CallToAction':
            if (section.buttonLink && section.buttonText) {
              return (
                <section
                  key={section.type}
                  className={`${styles.CallToAction} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}
                  style={{ animationDelay: `${index * 0.2}s` }}
                >
                  <h3>{section.title}</h3>
                  <Link to={section.buttonLink} className={styles.Button}>
                    {section.buttonText}
                  </Link>
                </section>
              );
            }
            else return null;
          default:
            return null;
        }
      })}
    </div>
  );
}

export default AboutUsPage;