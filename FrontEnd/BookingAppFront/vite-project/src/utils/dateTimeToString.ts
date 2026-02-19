interface FormattedDate{
    monthName:string;
    time:string;
    dayNumber:number;
}
function dateTimeToString(dateTime:string) : FormattedDate{
    const eventDate = new Date(dateTime);

    const monthName = eventDate.toLocaleDateString('en-US', {month:'long'});
    const dayNumber = eventDate.getDate();
    const time = eventDate.toLocaleTimeString('en-US', { hour: 'numeric', minute: '2-digit', hour12: true });
    return {
        monthName,
        time,
        dayNumber
    }
}
export default dateTimeToString; 