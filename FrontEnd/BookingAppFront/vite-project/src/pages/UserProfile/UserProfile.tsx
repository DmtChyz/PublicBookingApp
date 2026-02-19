import { useState, useEffect } from 'react';
import styles from './UserProfile.module.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { ICONS } from '../../config/icons';
import { useAuth } from '../../context/AuthContext/AuthContext';
import { useNavigate } from 'react-router-dom';
import LoadingScreen from '../../components/LoadingScreen/LoadingScreen';
import { getUserProfile, updateUserProfile } from '../../api/users/users';
import { useNotification } from '../../context/NotificationContext/NotificationContext';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import BookingList from '../../components/BookingList/BookingList';
import UserEventsList from '../../components/UserEventsList/UserEventsList';

function UserProfile() {
    const { token, logout } = useAuth();
    const { showNotification } = useNotification();
    const navigate = useNavigate();
    const queryClient = useQueryClient();

    const [isEditing, setIsEditing] = useState(false);
    const [editForm, setEditForm] = useState({ username: '' });
    const [isVisible, setIsVisible] = useState(false);

    useEffect(() => {
        setIsVisible(true);
    }, []);

    const { data: user, isLoading: isUserLoading, error } = useQuery({
        queryKey: ['userProfile'],
        queryFn: () => getUserProfile(),
        enabled: !!token,
    });

    useEffect(() => {
        if (!token) {
            navigate('/login');
        }
    }, [token, navigate]);
    
    useEffect(() => {
        if (user) {
            setEditForm({ username: user.username });
        }
    }, [user]);

    const updateUserMutation = useMutation({
        mutationFn: (newUsername: string) => updateUserProfile({ username: newUsername }),
        onSuccess: () => {
            showNotification("Profile updated successfully", "success");
            setIsEditing(false);
            queryClient.invalidateQueries({ queryKey: ['userProfile'] });
        },
        onError: (err: Error) => {
            showNotification(err.message || "Failed to save changes", "error");
        }
    });

    const handleSaveClick = () => {
        if (!editForm.username.trim()) {
            showNotification("Username cannot be empty", "error");
            return;
        }
        updateUserMutation.mutate(editForm.username);
    };

    if (isUserLoading) {
        return <LoadingScreen />;
    }

    if (error) {
        return (
            <div className={styles.container}>
                <div className={styles.errorCard}>
                    <h2 className={styles.errorTitle}>Error</h2>
                    <p>{error.message}</p>
                    <button className={styles.logoutBtn} onClick={logout}>Logout</button>
                </div>
            </div>
        );
    }

    return (
        <div className={styles.container}>
            <div className={`${styles.profileCard} ${styles.fadeInOnLoad} ${isVisible ? styles.visible : ''}`}>
                <div className={styles.iconContainer}>
                    <FontAwesomeIcon icon={ICONS.user} className={styles.bigIcon} />
                </div>
                
                <h1 className={styles.title}>User Profile</h1>
                
                <div className={styles.infoGroup}>
                    <label>Username</label>
                    {isEditing ? (
                        <input 
                            type="text" 
                            className={styles.inputField}
                            value={editForm.username}
                            onChange={(e) => setEditForm({...editForm, username: e.target.value})}
                            disabled={updateUserMutation.isPending}
                        />
                    ) : (
                        <div className={styles.value}>{user?.username || "N/A"}</div>
                    )}
                </div>

                <div className={styles.infoGroup}>
                    <label>Email</label>
                    <div className={`${styles.value} ${styles.readOnly}`}>{user?.email || "N/A"}</div>
                </div>

                <div className={styles.actions}>
                    {isEditing ? (
                        <>
                            <button 
                                className={styles.saveBtn} 
                                onClick={handleSaveClick}
                                disabled={updateUserMutation.isPending}
                            >
                                {updateUserMutation.isPending ? "Saving..." : "Save Changes"}
                            </button>
                            <button 
                                className={styles.cancelBtn} 
                                onClick={() => setIsEditing(false)}
                                disabled={updateUserMutation.isPending}
                            >
                                Cancel
                            </button>
                        </>
                    ) : (
                        <>
                            <button className={styles.editBtn} onClick={() => setIsEditing(true)}>
                                Edit Profile
                            </button>
                            <button className={styles.logoutBtn} onClick={logout}>
                                Logout
                            </button>
                        </>
                    )}
                </div>
            </div>
            <BookingList />
            <UserEventsList/>
        </div>
    );
}

export default UserProfile;