import { jwtDecode } from 'jwt-decode';

interface DecodedToken {
  exp: number;
  [key: string]: any;
}

export function getAuthToken(): string | null {
  const token = localStorage.getItem('token');

  if (!token) {
    return null;
  }

  try {
    const decodedToken: DecodedToken = jwtDecode(token);
    const currentTime = Date.now() / 1000; // Convert to seconds

    if (decodedToken.exp < currentTime) {
      localStorage.removeItem('token');
      return null; // Token is expired
    }
  } catch (error) {
    localStorage.removeItem('token');
    return null; // Token is invalid
  }

  return token;
}