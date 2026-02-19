import { getAuthToken } from '../../utils/auth';

const BASE_URL = "https://api20260106040424-cefehuf4hfgrgybr.germanywestcentral-01.azurewebsites.net/api";

export interface EventSummary {
    imageUrl: string;
    id: number;
    title: string;
    eventDate: string;
    price: number;
}

export interface BookingDto {
    id: number;
    eventSummary: EventSummary;
    createdAt: string;
    status: string;
    notes?: string;
    numberOfSeats: number;
}

export interface CreateBookingRequest {
    eventId: number;
    numberOfSeats: number;
    notes?: string;
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

export const createBooking = async (bookingData: CreateBookingRequest): Promise<BookingDto> => {
    const token = getAuthToken();
    const response = await fetch(`${BASE_URL}/booking`, { 
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(bookingData)
    });
    await handleErrors(response);
    return response.json();
};

export const getMyBookings = async (): Promise<BookingDto[]> => {
    const token = getAuthToken();
    const response = await fetch(`${BASE_URL}/booking/mybookings`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    await handleErrors(response);
    return response.json();
};

export const deleteBooking = async (bookingId: number): Promise<BookingDto> => {
    const token = getAuthToken();
    const response = await fetch(`${BASE_URL}/booking/${bookingId}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    });
    await handleErrors(response);
    return response.json();
};