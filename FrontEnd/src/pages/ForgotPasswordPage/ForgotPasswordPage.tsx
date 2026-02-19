import { useState } from 'react';
import { Link } from 'react-router-dom';
import { useMutation } from '@tanstack/react-query';
import { useNotification } from '../../context/NotificationContext/NotificationContext';
import { forgotPassword, type ForgotPasswordBody } from '../../api/authorization/authorization';
import styles from './ForgotPasswordPage.module.css';

function ForgotPasswordPage() {
    const { showNotification } = useNotification();
    const [email, setEmail] = useState('');

    const { mutate: performForgotPassword, isPending, isSuccess, error } = useMutation({
        mutationFn: (data: ForgotPasswordBody) => forgotPassword(data),
        onSuccess: () => {
            showNotification("If an account exists, a reset link has been sent.", "success");
        },
        onError: (err: Error) => {
            console.error(err);
        }
    });

    function handleSubmit(event: React.FormEvent) {
        event.preventDefault();
        performForgotPassword({ email });
    };

    return (
        <div className={styles.pageWrapper}>
            <div className={`${styles.loginCard} ${styles.fadeInOnLoad} ${styles.visible}`}>
                <h2 className={styles.title}>Forgot Password</h2>
                <p className={styles.subtitle}>Enter your email to receive a reset link</p>

                <form onSubmit={handleSubmit} className={styles.formContainer}>
                    
                    {error && <div className={styles.errorMessage}>{error.message}</div>}

                    {isSuccess ? (
                        <p className={styles.successMessage}>Please check your inbox for the reset link.</p>
                    ) : (
                        <>
                            <div className={styles.fieldBlock}>
                                <label>Email</label>
                                <input
                                    type="email"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    placeholder="Enter your registered email"
                                    required
                                />
                            </div>

                            <button type="submit" className={styles.submitBtn} disabled={isPending}>
                                {isPending ? 'Sending Link...' : 'Send Reset Link'}
                            </button>
                        </>
                    )}
                </form>

                <div className={styles.footerLink}>
                    Remembered your password? <Link to="/login">Login here</Link>
                </div>
            </div>
        </div>
    );
}

export default ForgotPasswordPage;