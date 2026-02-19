import { getAuthToken } from '../../utils/auth';

const BASE_URL = "https://api20260106040424-cefehuf4hfgrgybr.germanywestcentral-01.azurewebsites.net/api";

export interface UserProfileData {
    id: string;
    username: string;
    email: string;
}

export interface UpdateUserData {
    username: string;
}

async function handleErrors(response: Response) {
    if (!response.ok) {
        let message = `Request failed with status ${response.status}.`;
        try {
            const errorData = await response.json();
            if (errorData.errors) {
                const firstErrorKey = Object.keys(errorData.errors)[0];
                message = errorData.errors[firstErrorKey][0];
            } else if (errorData.message) {
                message = errorData.message;
            }
        } catch (e) {}
        throw new Error(message);
    }
}

export const getUserProfile = async (): Promise<UserProfileData> => {
    const token = getAuthToken();
    const response = await fetch(`${BASE_URL}/users/profile`, {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    await handleErrors(response);
    return response.json();
};

export const updateUserProfile = async (data: UpdateUserData): Promise<UserProfileData> => {
    const token = getAuthToken();
    const response = await fetch(`${BASE_URL}/users/profile`, {
        method: 'PATCH', 
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(data)
    });
    await handleErrors(response);
    return response.json();
};