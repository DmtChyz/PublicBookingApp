export interface Address {
    country: string;
    countryCode: string;
    city: string;
    venue: string;
}

export interface Event {
    id: number;
    title: string;
    description: string;
    eventDate: string;
    imageUrl: string;
    address: Address;
    price: number;
    maxAttendees: number;
}