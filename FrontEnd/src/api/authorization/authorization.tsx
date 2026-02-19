export type RegisterCredentials = {
    email: string;
    username: string;
    password: string;
};

export type LoginCredentials = {
    identifier: string;
    password: string;
}

export type TokenResponse = {
    token: string;
}

export interface ForgotPasswordBody {
  email: string;
}

export interface ResetPasswordBody {
  email: string;
  token: string;
  newPassword: string;
}

const BASE_URL = 'https://api20260106040424-cefehuf4hfgrgybr.germanywestcentral-01.azurewebsites.net/api';

async function handleErrors(response: Response) {
    if (!response.ok) {
        let message = `Request failed with status ${response.status}.`;
        try {
            const errorData = await response.json();
            if (errorData.errors) {
                const firstErrorKey = Object.keys(errorData.errors)[0];
                message = errorData.errors[firstErrorKey].join(', ');
            } else if (errorData.message || errorData.error) {
                message = errorData.message || errorData.error;
            } else if (typeof errorData === 'string') {
                message = errorData;
            }
        } catch (e) {}
        throw new Error(message);
    }
}

export const loginUser = async (credentials: LoginCredentials): Promise<TokenResponse> => {
    const response = await fetch(`${BASE_URL}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(credentials)
    });
    await handleErrors(response);
    return response.json();
};

export const registerUser = async (credentials: RegisterCredentials): Promise<TokenResponse> => {
    const response = await fetch(`${BASE_URL}/auth/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(credentials)
    });
    await handleErrors(response);
    return response.json();
};

export const forgotPassword = async (data: ForgotPasswordBody): Promise<{ success: true }> => {
  const response = await fetch(`${BASE_URL}/auth/forgot-password`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  await handleErrors(response);
  return { success: true };
};

export const resetPassword = async (data: ResetPasswordBody): Promise<{ success: true }> => {
  const response = await fetch(`${BASE_URL}/auth/reset-password`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  await handleErrors(response);
  return { success: true };
};