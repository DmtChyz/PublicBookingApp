import { getAuthToken } from '../../utils/auth';
import type { Event } from '../../types/Event';
// const BASE_URL = "https://localhost:7061/api";
const BASE_URL = "https://api20260106040424-cefehuf4hfgrgybr.germanywestcentral-01.azurewebsites.net/api";

interface AddressDTO {
  country: string;
  countryCode: string;
  city: string;
  venue: string;
}

export interface CreateEventData {
  title: string;
  description: string;
  eventDate: string; 
  maxAttendees: number;
  imageUrl: string;
  price: number;
  address: AddressDTO;
}

export interface UpdateEventData {
  title?: string;
  description?: string;
  eventDate?: string;
  maxAttendees?: number;
  imageUrl?: string;
  address?: Partial<AddressDTO>;
}
export interface GenerateDescriptionData {
  title: string | null;
  city: string | null;
  country: string | null;
  price: number | null;
  userPrompt: string;
}

class HttpError extends Error {
    status: number;
    constructor(status: number, message: string) {
        super(message);
        this.status = status;
    }
}

async function handleErrors(response: Response) {
    if (!response.ok) {
        let message = `Request failed with status ${response.status}.`;
        const responseClone = response.clone();

        try {
            const errorData = await responseClone.json();
            if (errorData && errorData.errors) {
                if (typeof errorData.errors === 'string') {
                    message = errorData.errors;
                } else {
                    const firstErrorKey = Object.keys(errorData.errors)[0];
                    const errorMessages = errorData.errors[firstErrorKey];
                    if (Array.isArray(errorMessages) && errorMessages.length > 0) {
                        message = errorMessages[0];
                    }
                }
            } else if (errorData.message) {
                message = errorData.message;
            }
        } 
        catch (e) {
            try {
                const errorText = await response.text();
                if (errorText) {
                    message = errorText;
                }
            } 
            catch (textError) 
            {

            }
        }
        throw new HttpError(response.status, message);
    }
}
export const getAllEvents = async (
  page: number = 1,
  pageSize: number = 10,
  sortBy?: 'date' | 'price',
  sortOrder?: 'asc' | 'desc'
): Promise<Event[]> => {
  const params = new URLSearchParams({
    page: String(page),
    pageSize: String(pageSize),
  });

  if (sortBy) {
    params.append('sortBy', sortBy);
  }
  if (sortOrder) {
    params.append('sortOrder', sortOrder);
  }

  const response = await fetch(`${BASE_URL}/event?${params.toString()}`);
  await handleErrors(response);
  return response.json();
};

export const getEventById = async (id: string): Promise<Event> => {
  const response = await fetch(`${BASE_URL}/event/${id}`);
  await handleErrors(response);
  return response.json();
};

export const createEvent = async (eventData: CreateEventData): Promise<Event> => {
  const token = getAuthToken();
  const response = await fetch(`${BASE_URL}/event`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(eventData),
  });
  await handleErrors(response);
  return response.json();
};

export const deleteEvent = async (eventId: number): Promise<{ success: true }> => {
  const token = getAuthToken();
  const response = await fetch(`${BASE_URL}/event/${eventId}`, {
    method: 'DELETE',
    headers: { 'Authorization': `Bearer ${token}` }
  });
  await handleErrors(response);
  return { success: true };
};

export const updateEvent = async ({ eventId, eventData }: { eventId: number, eventData: UpdateEventData }): Promise<Event> => {
    const token = getAuthToken();
    const response = await fetch(`${BASE_URL}/event/${eventId}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(eventData)
    });
    await handleErrors(response);
    return response.json();
};

export const getMyEvents = async (): Promise<Event[]> => {
  const token = getAuthToken();
  const response = await fetch(`${BASE_URL}/event/my-events`, {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  });
  await handleErrors(response);
  return response.json();
};

export const generateEventDescription = async (data: GenerateDescriptionData): Promise<string> => {
  const token = getAuthToken();
  const response = await fetch(`${BASE_URL}/event/generateDescription`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify(data),
  });
  await handleErrors(response);
  return response.text(); 
};