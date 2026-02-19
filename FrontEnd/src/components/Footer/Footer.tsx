import styles from './Footer.module.css';

function Footer(){
    return(
        <footer className={styles.Footer}>
            <div className={styles.FooterContentBox}>
                <a href={''}>Contacts</a>
                <a href={''}>Support</a>
                <a href={''}>For contributors</a>
            </div>
            <p className={styles.CopyrightText}>
                &copy; 2025 BookingApp. All Rights Reserved.
            </p>
    </footer>
    )
}
export default Footer;