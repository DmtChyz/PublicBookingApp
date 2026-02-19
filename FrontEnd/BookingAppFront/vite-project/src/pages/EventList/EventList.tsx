import React, { useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import styles from './EventList.module.css';
import { getAllEvents } from '../../api/events/events';
import EventCard from '../../components/EventCard/EventCard';
import LoadingScreen from '../../components/LoadingScreen/LoadingScreen';
import { useQuery } from '@tanstack/react-query';
import type { Event } from '../../types/Event';

type SortByType = 'date' | 'price';
type SortOrderType = 'asc' | 'desc';

function EventList() {
    const [searchParams, setSearchParams] = useSearchParams();

    const currentPage = parseInt(searchParams.get('page') || '1');
    const sortBy = (searchParams.get('sortBy') as SortByType) || 'date';
    const sortOrder = (searchParams.get('sortOrder') as SortOrderType) || 'asc';

    const PAGE_SIZE = 10;

    const { data: events = [], isLoading, error, refetch } = useQuery<Event[]>({
        queryKey: ['events', currentPage, sortBy, sortOrder],
        queryFn: () => getAllEvents(currentPage, PAGE_SIZE, sortBy, sortOrder),
        retry: 1,
    });
    
    useEffect(() => {
        window.scrollTo(0, 0);
    }, [currentPage, sortBy, sortOrder]);

    const handleSortChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const { name, value } = e.target;
        const newParams = new URLSearchParams(searchParams);
        newParams.set(name, value);
        newParams.set('page', '1');
        setSearchParams(newParams);
    };

    const handlePreviousPage = () => {
        if (currentPage > 1) {
            const newParams = new URLSearchParams(searchParams);
            newParams.set('page', (currentPage - 1).toString());
            setSearchParams(newParams);
        }
    };

    const handleNextPage = () => {
        if (events.length === PAGE_SIZE) {
            const newParams = new URLSearchParams(searchParams);
            newParams.set('page', (currentPage + 1).toString());
            setSearchParams(newParams);
        }
    };
    
    if (error) {
        return (
            <div className={styles.listPageContainer}>
                <div className={styles.errorContainer}>
                    <h3 className={styles.errorTitle}>Oops! Something went wrong.</h3>
                    <p className={styles.errorMessage}>{error.message}</p>
                    <button onClick={() => refetch()} className={styles.retryBtn}>
                        Try Again
                    </button>
                </div>
            </div>
        );
    }

    if (isLoading) {
        return <LoadingScreen />;
    }

    if (events.length === 0) {
        return (
            <div className={styles.listPageContainer}>
                <div className={styles.emptyContainer}>
                    <h3 className={styles.emptyTitle}>No Events Found</h3>
                    <p className={styles.emptyMessage}>
                        It looks like there are no events matching your criteria.
                    </p>
                    {currentPage > 1 && (
                        <button onClick={handlePreviousPage} className={styles.retryBtn}>
                            Go Back
                        </button>
                    )}
                </div>
            </div>
        );
    }

    return (
        <div className={styles.listPageContainer}>
            <div className={styles.controlsContainer}>
                <div className={styles.sortingControlGroup}>
                    <label htmlFor="sortBy" className={styles.sortLabel}>Sort by</label>
                    <select id="sortBy" name="sortBy" value={sortBy} onChange={handleSortChange} className={styles.sortSelect}>
                        <option value="date">Date</option>
                        <option value="price">Price</option>
                    </select>
                </div>
                <div className={styles.sortingControlGroup}>
                    <label htmlFor="sortOrder" className={styles.sortLabel}>Order</label>
                    <select id="sortOrder" name="sortOrder" value={sortOrder} onChange={handleSortChange} className={styles.sortSelect}>
                        <option value="asc">Ascending</option>
                        <option value="desc">Descending</option>
                    </select>
                </div>
            </div>
        
            <div className={styles.EventContainer}>
                {events.map((event, index) => (
                    <EventCard
                        key={event.id}
                        id={event.id.toString()}
                        imageUrl={event.imageUrl}
                        title={event.title}
                        price={event.price}
                        datetime={event.eventDate} 
                        address={event.address}
                        index={index}
                    />
                ))}
            </div>

            <div className={styles.paginationControls}>
                <button onClick={handlePreviousPage} disabled={currentPage === 1} className={styles.pageBtn}>
                    &larr; Previous
                </button>
                <span className={styles.pageNumber}>Page {currentPage}</span>
                <button onClick={handleNextPage} disabled={events.length < PAGE_SIZE} className={styles.pageBtn}>
                    Next &rarr;
                </button>
            </div>
        </div>
    );
}

export default EventList;