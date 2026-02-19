import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import styles from './RegisterForm.module.css';
import { useAuth } from '../../context/AuthContext/AuthContext';
import { useNotification } from '../../context/NotificationContext/NotificationContext';
import { useMutation } from '@tanstack/react-query';
import { registerUser, loginUser, type RegisterCredentials } from '../../api/authorization/authorization';

function RegisterForm() {
    const navigate = useNavigate();
    const { login } = useAuth();
    const { showNotification } = useNotification();
    
    const [formData, setFormData] = useState({
        email: '',
        username: '',
        password: '',
        confirmPassword: ''
    });

    const { mutate: performRegister, isPending, error } = useMutation({
        mutationFn: (credentials: RegisterCredentials) => registerUser(credentials),
        onSuccess: async () => {
            showNotification("Account created successfully!", "success");
            try {
                const loginData = await loginUser({ 
                    identifier: formData.username, 
                    password: formData.password 
                });
                login(loginData.token);
                navigate('/events');
            } catch (loginError) {
                showNotification("Registration successful, but auto-login failed. Please log in.", "error");
                navigate('/login');
            }
        },
        onError: (err: Error) => {
            console.error("Registration failed:", err);
        }
    });

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const getInputClass = (fieldName: keyof typeof formData) => {
        const value = formData[fieldName];
        if (error) return styles.inputError;
        if (fieldName === 'confirmPassword') {
            if (!value) return '';
            return value !== formData.password ? styles.inputError : styles.inputSuccess;
        }
        return value ? styles.inputSuccess : '';
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        
        if (formData.password !== formData.confirmPassword) {
            showNotification("Passwords do not match!", "error");
            return;
        }

        performRegister({
            email: formData.email,
            username: formData.username,
            password: formData.password
        });
    };

    return (
        <div className={styles.pageWrapper}>
            <div className={`${styles.loginCard} ${styles.fadeInOnLoad} ${styles.visible}`}>
                <h2 className={styles.title}>Join Us</h2>
                <p className={styles.subtitle}>Create your account today</p>
                
                <form onSubmit={handleSubmit} className={styles.formContainer}>
                    
                    {error && <div className={styles.errorMessage}>{error.message}</div>}
                    
                    <div className={styles.fieldBlock}>
                        <label>Email</label>
                        <input type="email" name="email" value={formData.email} onChange={handleChange} className={getInputClass('email')} required />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label>Username</label>
                        <input type="text" name="username" value={formData.username} onChange={handleChange} className={getInputClass('username')} required />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label>Password</label>
                        <input type="password" name="password" value={formData.password} onChange={handleChange} className={getInputClass('password')} required />
                    </div>
                    <div className={styles.fieldBlock}>
                        <label>Confirm Password</label>
                        <input type="password" name="confirmPassword" value={formData.confirmPassword} onChange={handleChange} className={getInputClass('confirmPassword')} required />
                    </div>
                    
                    <button type="submit" className={styles.submitBtn} disabled={isPending}>
                        {isPending ? 'Registering...' : 'Register'}
                    </button>
                </form>

                <div className={styles.footerLink}>
                    Already have an account? <Link to="/login">Sign in here</Link>
                </div>
            </div>
        </div>
    );
}

export default RegisterForm;