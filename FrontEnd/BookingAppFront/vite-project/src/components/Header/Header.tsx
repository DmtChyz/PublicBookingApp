import styles from './Header.module.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { ICONS } from '../../config/icons';
import { useState, useEffect } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext/AuthContext';

function Header() {
    const currentPage = useLocation();
    const { token, logout } = useAuth();
    const [isMenuOpen, setMenuOpen] = useState(false);
    const navigate = useNavigate(); 

    useEffect(() => {
        setMenuOpen(false)
    }, [currentPage])

    const handleUserIconClick = () => {
        if (token) {
            navigate('/profile');
        } else {
            navigate('/login'); 
        }
    };

    return (
        <header className={styles.headerContainer}>
            <div className={styles.LogoContainer}>
                <div>BookingApp</div>
            </div>
            
            <div className={styles.ButtonContainer}>
                <div className={styles.Button}><Link to="/events">Events</Link></div>
                <div className={styles.Button}><Link to="/rights">Rights</Link></div>
                <div className={styles.Button}><Link to="/about">About Us</Link></div>
                {token &&(
                    <>
                        <div className={styles.Button}><Link to="/createEvent">Create Event</Link></div>
                    </>
                )}
                
                {!token && (
                    <div className={styles.Button}>
                        <Link to="/login">Log In</Link>
                    </div>
                )}
            </div>

            <div className={styles.AuthContainer}>
                <div 
                    className={`${styles.UserIcon} ${token ? styles.UserIconGreen : styles.UserIconRed}`} 
                    onClick={handleUserIconClick}
                    title={token ? "User Profile" : "Not Logged In"}
                >
                    <FontAwesomeIcon icon={ICONS.user} /> 
                </div>

                <button className={styles.MenuButton} onClick={() => setMenuOpen(!isMenuOpen)}> 
                    <FontAwesomeIcon className={styles.menuIcon} icon={ICONS.menu} /> 
                </button>
                
                {isMenuOpen ? (
                    <div className={styles.PopUpMenu}>
                        <Link to='/contacts'>Contacts</Link>
                        <Link to='/support'>Support</Link>
                        <Link to='/for-contributors'>For contributors</Link>
                        
                        {token && (
                            <>
                                <hr />
                                <div onClick={logout} className={styles.MobileLogout}>Logout</div>
                            </>
                        )}
                        
                        <div className={styles.ShowOnMobile}>
                            <hr></hr>
                            {!token && (
                                <>
                                    <Link to="/login">Log In</Link>
                                    <Link to="/register">Register</Link>
                                    <hr></hr>
                                </>
                            )}
                            <Link to="/events">Events</Link>
                            <Link to="/rights">Rights</Link>
                            <Link to="/about">About us</Link>
                        </div>
                    </div>
                ) : null}
            </div>
        </header>
    );
}

export default Header;