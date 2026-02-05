export const formatDate = (dateString: string): string => {
    if (!dateString) return '';

    const date = new Date(dateString);

    if (isNaN(date.getTime())) return dateString;

    return new Intl.DateTimeFormat('pt-BR', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false
    }).format(date).replace(',', ' -');
};

export const formatPhone = (value: string) => {
    const digits = value.replace(/\D/g, '');

    if (digits.length === 0) {
        return "";
    } else if (digits.length <= 2) {
        return `(${digits.slice(0, 2)}`;
    } else if (digits.length <= 7) {
        return `(${digits.slice(0, 2)}) ${digits.slice(2)}`;
    } else {
        return `(${digits.slice(0, 2)}) ${digits.slice(2, 7)}-${digits.slice(7, 11)}`;
    }
};

export const formatRA = (value: string) => {
    const digits = value.replace(/\D/g, '');

    // 12.345.678-9
    if (digits.length === 0) {
        return "";
    } else if (digits.length <= 2) {
        return `${digits.slice(0, 2)}`;
    } else if (digits.length <= 5) {
        return `${digits.slice(0, 2)}.${digits.slice(2)}`;
    } else if (digits.length <= 8) {
        return `${digits.slice(0, 2)}.${digits.slice(2, 5)}.${digits.slice(5, 8)}`;
    } else {
        return `${digits.slice(0, 2)}.${digits.slice(2, 5)}.${digits.slice(5, 8)}-${digits[8] || ''}`;
    }
};
