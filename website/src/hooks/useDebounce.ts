import { useState, useEffect } from "react";

/**
 * Hook personalizado para debouncing de valores.
 * @param value O valor a ser atrasado.
 * @param delay O atraso em milissegundos (padrão 500ms).
 * @returns O valor atrasado.
 */
export function useDebounce<T>(value: T, delay: number = 500): T {
    const [debouncedValue, setDebouncedValue] = useState<T>(value);

    useEffect(() => {
        // Configura o timer para atualizar o valor debounced
        const handler = setTimeout(() => {
            setDebouncedValue(value);
        }, delay);

        // Limpa o timer se o valor mudar ou se o componente desmontar
        return () => {
            clearTimeout(handler);
        };
    }, [value, delay]);

    return debouncedValue;
}
