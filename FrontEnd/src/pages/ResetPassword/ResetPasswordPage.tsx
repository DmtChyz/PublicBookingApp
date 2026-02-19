import { useState } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { useMutation } from '@tanstack/react-query';
import { useNotification } from '../../context/NotificationContext/NotificationContext';
import { resetPassword } from '../../api/authorization/authorization';
import styles from './ResetPasswordPage.module.css';

function ResetPasswordPage() {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();
    const { showNotification } = useNotification();
    
    const [error, setError] = useState<string | null>(null);

    const token = searchParams.get('token');
    const email = searchParams.get('email');

    const { mutate, isPending } = useMutation({
        mutationFn: resetPassword,
        onSuccess: () => {
            showNotification("Your password has been reset successfully!", "success");
            navigate('/login');
        },
        onError: (err: Error) => {
            setError(err.message);
        }
    });

    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        setError(null);

        const formData = new FormData(event.currentTarget);
        const newPassword = formData.get('newPassword') as string;
        const confirmPassword = formData.get('confirmPassword') as string;

        if (newPassword !== confirmPassword) {
            setError("Passwords do not match.");
            return;
        }

        if (token && email) {
            mutate({ email, token, newPassword });
        }
    };

    if (!token || !email) {
        return (
            <div className={styles.pageWrapper}>
                <div className={styles.loginCard}>
                    <h2 className={styles.title}>Invalid Link</h2>
                    <p className={styles.subtitle}>This password reset link is invalid or has expired.</p>
                </div>
            </div>
        );
    }

    return (
        <div className={styles.pageWrapper}>
            <div className={`${styles.loginCard} ${styles.fadeInOnLoad} ${styles.visible}`}>
                <h2 className={styles.title}>Reset Your Password</h2>
                <p className={styles.subtitle}>Please enter your new password</p>

                <form onSubmit={handleSubmit} className={styles.formContainer}>
                    
                    {error && <div className={styles.errorMessage}>{error}</div>}

                    <div className={styles.fieldBlock}>
                        <label>New Password</label>
                        <input
                            type="password"
                            name="newPassword"
                            placeholder="Enter your new password"
                            required
                        />
                    </div>
                    
                    <div className={styles.fieldBlock}>
                        <label>Confirm New Password</label>
                        <input
                            type="password"
                            name="confirmPassword"
                            placeholder="Confirm your new password"
                            required
                        />
                    </div>

                    <button type="submit" className={styles.submitBtn} disabled={isPending}>
                        {isPending ? 'Resetting...' : 'Reset Password'}
                    </button>
                </form>
            </div>
        </div>
    );
}

export default ResetPasswordPage;