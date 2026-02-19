import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom'; 
import { useAuth } from '../../context/AuthContext/AuthContext';
import { useNotification } from '../../context/NotificationContext/NotificationContext';
import { useMutation } from '@tanstack/react-query';
import { loginUser, type LoginCredentials } from '../../api/authorization/authorization';
import styles from './LoginForm.module.css';

function LoginForm() {
    const navigate = useNavigate();
    const { login } = useAuth();
    const { showNotification } = useNotification();
    
    const [identifier, setIdentifier] = useState(''); 
    const [password, setPassword] = useState('');
    
    const { mutate: performLogin, isPending, error } = useMutation({
        mutationFn: (credentials: LoginCredentials) => loginUser(credentials),
        onSuccess: (data) => {
            login(data.token);
            showNotification("Welcome back!", "success");
            navigate('/about');
        },
        onError: (err: Error) => {
            console.error(err);
        }
    });

    function handleSubmit(event: React.FormEvent){
        event.preventDefault();
        performLogin({ identifier, password });
    };

    return (
        <div className={styles.pageWrapper}>
            <div className={`${styles.loginCard} ${styles.fadeInOnLoad} ${styles.visible}`}>
                <h2 className={styles.title}>Welcome Back</h2>
                <p className={styles.subtitle}>Please sign in to continue</p>
                
                <form onSubmit={handleSubmit} className={styles.formContainer}>
                    
                    {error && <div className={styles.errorMessage}>{error.message}</div>}
                    
                    <div className={styles.fieldBlock}>
                        <label>Username or Email</label>
                        <input 
                            type="text"
                            value={identifier} 
                            onChange={(e) => setIdentifier(e.target.value)} 
                            placeholder="Enter your username or email"
                            required
                        />
                    </div>
                    
                    <div className={styles.fieldBlock}>
                        <label>Password</label>
                        <input 
                            type="password" 
                            value={password} 
                            onChange={(e) => setPassword(e.target.value)}
                            placeholder="Enter your password"
                            required
                        />
                    </div>
                    
                    <button type="submit" className={styles.submitBtn} disabled={isPending}>
                        {isPending ? 'Logging in...' : 'Login'}
                    </button>
                </form>

                <div className={styles.footerLink}>
                    <p>Don't have an account? <Link to="/register">Create one here</Link></p>
                    <p>Forgot password? <Link to="/forgot-password">Reset here</Link></p>
                </div>
            </div>
        </div>
    );
}

export default LoginForm;