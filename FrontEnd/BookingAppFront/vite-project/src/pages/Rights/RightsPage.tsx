import { useState, useEffect } from 'react'; 
import styles from './RightsPage.module.css';

const sectionsData = [
  {
    title: '1. Introduction',
    content: [
      'Welcome to BookingApp. These terms and conditions outline the rules and regulations for the use of our website and services. By accessing this website, we assume you accept these terms. Do not continue to use BookingApp if you do not agree to all of the terms and conditions stated on this page.'
    ]
  },
  {
    title: '2. User Accounts',
    content: [
      'When you create an account with us, you must provide information that is accurate, complete, and current at all times. Failure to do so constitutes a breach of the Terms, which may result in immediate termination of your account on our Service.',
      'You are responsible for safeguarding the password that you use to access the Service and for any activities or actions under your password.',
      'You must notify us immediately upon becoming aware of any breach of security or unauthorized use of your account.'
    ]
  },
  {
    title: '3. Ticket Purchases',
    content: [
      `All ticket purchases made through the platform are subject to the terms set by the event organizers, in addition to our own. All sales are considered final. Refunds and exchanges are only offered in accordance with the organizer's policy or as required by law.`,
      `We act as an intermediary, and are not responsible for the execution of the event itself.`
    ]
  },
  {
    title: '4. Intellectual Property',
    content: [
      'The Service and its original content, features and functionality are and will remain the exclusive property of BookingApp and its licensors. The Service is protected by copyright, trademark, and other laws of both the United States and foreign countries.'
    ]
  },
  {
    title: '5. Termination',
    content: [
      'We may terminate or suspend your account immediately, without prior notice or liability, for any reason whatsoever, including without limitation if you breach the Terms. Upon termination, your right to use the Service will immediately cease.'
    ]
  }
];

function RightsPage() {
  const [isLoaded, setIsLoaded] = useState(false);

  useEffect(() => {
    setIsLoaded(true);
  }, []);

  return (
    <div className={styles.rightsContainer}>
      <div className={styles.Header}> 
        <h1>Terms of Service</h1>
        <p>Last Updated: October 9, 2025</p>
      </div>

      <div className={styles.ContentBox}>
        {sectionsData.map((section, index) => (
          <section 
            key={section.title} 
            className={`${styles.section} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`} 
            style={{ animationDelay: `${index * 0.2}s` }}
          >
            <div className={styles.sectionTitle}>
              <h3>{section.title}</h3>
            </div>
            <div className={`${styles.sectionContent} ${styles.sectionTiny}`}>
              {section.content.map((paragraph, pIndex) => (
                <p key={pIndex}>{paragraph}</p>
              ))}
            </div>
          </section>
        ))}
      </div>
    </div>
  );
}

export default RightsPage;