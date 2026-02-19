import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import styles from './Support.module.css';
import { useState, useEffect } from 'react';
import { ICONS } from '../../config/icons';
import { Link } from 'react-router-dom';

const supportPageData = [
  {
    type: 'Hero',
    id: 'sph-1',
    title: 'How can we help?',
    subtitlePart1: 'Find answers ',
    subtitlePart2: 'to your questions',
    subtitlePart3: ' below, or ',
    subtitlePart4: 'get in touch',
    subtitlePart5: ' with our team.',
  },
  {
    type: 'FaqCategories',
    id: 'spc-1',
    title: 'Browse by Category',
    categories: [
      { id: 'cat1', name: 'Account & Billing', icon: ICONS.idCard },
      { id: 'cat2', name: 'Booking Process', icon: ICONS.calendar },
      { id: 'cat3', name: 'Technical Issues', icon: ICONS.gear }
    ]
  },
  {
    type: 'AccordionGroup',
    id: 'spa-1',
    title: 'Frequently Asked Questions',
    questions: [
      {
        id: 'q1',
        question: 'How do I reset my password?',
        answer: 'To reset your password, go to the login page and click the "Forgot Password" link. You will receive an email with instructions on how to set a new password.',
        categoryId: 'cat3'
      },
      {
        id: 'q2',
        question: 'Can I cancel a booking?',
        answer: 'Yes, you can cancel a booking up to 24 hours before the scheduled time. To do so, navigate to "My Bookings" in your account and select the booking you wish to cancel.',
        categoryId: 'cat1'
      },
      {
        id: 'q3',
        question: 'What payment methods do you accept?',
        answer: 'We accept all major credit cards, including Visa, Mastercard, and American Express. We also support payments through PayPal for your convenience.',
        categoryId: 'cat2'
      },
      {
        id: 'q4',
        question: 'Is my personal information secure?',
        answer: 'Absolutely. We use industry-standard SSL encryption to protect your data. Your privacy and security are our top priorities.',
        categoryId: 'cat3'      
      }
    ]
  },
  {
    type: 'ContactPrompt',
    id: 'spp-1',
    title: "Can't find your answer?",
    text: 'Our team is here to help. Get in touch with us directly for any further questions.',
    buttonText: 'Contact Us',
    buttonLink: '/contacts'
  }
];

function SupportPage() {
  const [isLoaded, setIsLoaded] = useState(false);
  const [openAccordionId, setOpenAccordionId] = useState<string | null>(null);
  const [selectedCategoryId, setSelectedCategoryId] = useState<string | null>(null);
  const [searchBarInfo,setSeacrhBarInfo] = useState('');

  const handleAccordionClick = (id: string) => {
    setOpenAccordionId(openAccordionId === id ? null : id);
  };

  useEffect(() => {
    setIsLoaded(true);
  }, []);

  return (
    <div className={styles.supportContainer}>
      {supportPageData.map((section, index) => {
        switch (section.type) {
          case "Hero":
            return (
              <section
                key={section.id}
                className={`${styles.heroSection} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}
                style={{ animationDelay: `${index * 0.2}s` }}>
                <h1>{section.title}</h1>
                <h2>
                  {section.subtitlePart1}
                  <strong>{section.subtitlePart2}</strong>
                  {section.subtitlePart3}
                  <strong>{section.subtitlePart4}</strong>
                  {section.subtitlePart5}
                </h2>
                <form className={styles.searchBar}>
                    <input 
                      className={styles.searchInput} 
                      type="text" 
                      placeholder="Seach for answers" 
                      onChange={(e) => setSeacrhBarInfo(e.target.value)}>
                    </input>
                </form>
              </section>
            );
          case "FaqCategories":
            return (
              <section
                key={section.id}
                className={`${styles.faqCategories} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}
                style={{ animationDelay: `${index * 0.2}s` }}>
                <h2>{section.title}</h2>
                <div className={styles.faqBox}>
                  {section.categories && section.categories.map((category) => (
                    <div
                      key={category.id}
                      className={styles.categoryCard}
                      onClick={() => setSelectedCategoryId(category.id)}>
                      <div className={styles.categoryIconWrapper}>
                        <FontAwesomeIcon icon={category.icon} className={styles.icon}/>
                      </div>
                      <h3>{category.name}</h3>
                    </div>
                  ))}
                </div>
              </section>
            );
          case "AccordionGroup":{
                  const questionsToShow = section.questions
                    // If a category selected it only keeps questions with a matching categoryId
                    ?.filter(q => !selectedCategoryId || q.categoryId === selectedCategoryId)

                    // Starts right after the first filter
                    ?.filter(q => {
                        const term = searchBarInfo.toLowerCase();
                        const questionText = q.question.toLowerCase();
                        const answerText = q.answer.toLowerCase();
                        return questionText.includes(term) || answerText.includes(term);
                    });
                    return (
                        <section
                            key={section.id}
                            className={`${styles.accordionGroup} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}
                            style={{ animationDelay: `${index * 0.2}s` }}>
                            
                            <h2>{section.title}</h2>
                            
                            <div className={styles.popUpBox}>
                              {questionsToShow && questionsToShow.length > 0 ? 
                              (
                                  questionsToShow.map((question) => (
                                      <div
                                          key={question.id}
                                          className={`${styles.accordionItem} ${openAccordionId === question.id ? styles.open : ''}`}
                                      >
                                          <button
                                              className={styles.accordionQuestion}
                                              onClick={() => handleAccordionClick(question.id)}
                                          >
                                              <span>{question.question}</span>
                                              <div className={styles.iconContainer}>
                                                  <span className={styles.plus}>[+]</span>
                                                  <span className={styles.minus}>[-]</span>
                                              </div>
                                          </button>
                                          {openAccordionId === question.id && (
                                              <div className={styles.accordionAnswer}>
                                                  <p>{question.answer}</p>
                                              </div>
                                          )}
                                      </div>
                                  ))
                              ) 
                              : 
                              ( 
                                <div className={`${styles.noResults} ${styles.fadeInOnLoad} ${isLoaded ? styles.visible : ''}`}>
                                  {selectedCategoryId != null ? (
                                      <p>
                                          {(() => {
                                              switch (selectedCategoryId) {
                                                  case 'cat1':
                                                      return 'No results found in "Account & Billing"';
                                                  case 'cat2':
                                                      return 'No results found in "Booking Process"';
                                                  case 'cat3':
                                                      return 'No results found in "Technical Issues"';
                                                  default:
                                                      return 'No results found.';
                                              }
                                          })()}
                                      </p>
                                  ) : (
                                      <p>No questions match your current search.</p>
                                  )}
                              </div>
                              )}
                          </div>
                        </section>
                    );
}
          case'ContactPrompt':
            return(
              <section
                className={styles.ContactPrompt}
                key={section.title}>
                <h2>{section.title}</h2>
                <h3>{section.text}</h3>
                {section.buttonLink && section.buttonText && (
                <Link to={section.buttonLink} className={styles.contactButton}>
                  {section.buttonText}
                </Link>
                )}
              </section>
            );
          default:
            return null;
        }
      })}
    </div>
  )
}
export default SupportPage;