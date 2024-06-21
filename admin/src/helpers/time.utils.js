import moment from 'moment';

export function formatDate(datetime) {
    return moment(datetime).format('HH:mm DD/MM/YYYY');
}