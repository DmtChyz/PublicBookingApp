import styles from './ForContributos.module.css';
import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { ICONS } from '../../config/icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const forContributorsData = [
  {
    id: 'contrib-hero',
    type: 'hero',
    title: 'Join Our Platform as a Contributor',
    content:
      'Showcase your events to a dedicated audience. We provide the platform, you provide the experience.',
  },
  {
    id: 'contrib-steps',
    type: 'steps_section',
    title: 'Getting Started',
    steps: [
      {
        id: 'step-1',
        icon: ICONS.circleUser,
        title: '1. Create Your Account',
        content:
          "Use your standard BookingApp account to get started. If you don't have one, signing up is quick and easy. All contributors must agree to our standard Terms of Service.",
      },
      {
        id: 'step-2',
        icon: ICONS.clipboardList,
        title: '2. Prepare Your Event Details',
        content:
          'Before submission, make sure you have high-quality photos, a compelling description, date, location, and pricing information ready. The more detail you provide, the better your event will perform.',
      },
      {
        id: 'step-3',
        icon: ICONS.envelope,
        title: '3. Contact Us to Enable Contributor Access',
        content:
          'Once your account is ready, send an email to our partnership team at `partners@bookingapp.com` . Our team will review your request and grant you contributor permissions to post and manage your events.',
      },
    ],
  },
  {
    id: 'contrib-cta',
    type: 'cta',
    title: 'Ready to Begin?',
    content:
      'If you have any questions, visit our Support page to contact us directly.',
    buttonText: 'Go to Support Page',
    buttonLink: '/support',
  },
];

function ForContributorsPage() {
  const [isLoaded, setIsLoaded] = useState(false);

  useEffect(() => {
    setIsLoaded(true);
  }, []);

  return (
    <div className={styles.sectionContainer}>
      {forContributorsData.map((item, index) => {
        switch (item.type) {
          case 'hero':
            return (
              <section
                key={item.id}
                className={`${styles.heroSection} ${styles.fadeInOnLoad} ${
                  isLoaded ? styles.visible : ''
                }`}
                style={{ animationDelay: `${index * 0.2}s` }}
              >
                <h2>{item.title}</h2>
                <p>{item.content}</p>
              </section>
            );
          case 'steps_section':
            return (
              <section
                key={item.id}
                className={`
                ${styles.stepsSection} 
                ${styles.fadeInOnLoad} 
                ${isLoaded ? styles.visible : ''}
                `}
                style={{ animationDelay: `${index * 0.2}s` }}
                >
                <h2>{item.title}</h2>
                <div className={styles.stepsGrid}>
                  {item.steps?.map((step) => (
                    <div key={step.id} className={styles.stepItem}>
                        <div className={styles.stepHeader}>
                            <div className={styles.IconBox}><FontAwesomeIcon className={styles.icon} icon={step.icon}/></div>
                            <h3>{step.title}</h3>
                        </div>
                        <p>{step.content}</p>
                    </div>
                    ))}
                </div>
              </section>
            );
          case 'cta':
            return (
            <section
                key={item.id}
                className={`
                    ${styles.ctaSection} 
                    ${styles.fadeInOnLoad} ${
                    isLoaded ? styles.visible : ''
                }`}
                style={{ animationDelay: `${index * 0.2}s` }}
            >
                <h2>{item.title}</h2>
                <p>{item.content}</p>
                <Link to={item.buttonLink ?? '#'} className={styles.ctaButton}>
                {item.buttonText}
                </Link>
            </section>
            );
          default:
            return null;
        }
      })}
    </div>
  );
}

export default ForContributorsPage;