export const isValidEmail = (email: string): boolean => {
    if (!email || typeof email !== 'string') return false;

    return email.trim().length > 0 && email.includes('@');
};
