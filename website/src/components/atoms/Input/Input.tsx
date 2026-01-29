import { InputHTMLAttributes, forwardRef } from 'react';

interface InputProps extends InputHTMLAttributes<HTMLInputElement | HTMLTextAreaElement> {
  variant?: 'default' | 'error' | 'success';
  label?: string;
  error?: string;
  helperText?: string;
  multiline?: boolean;
  rows?: number;
}

export const Input = forwardRef<HTMLInputElement | HTMLTextAreaElement, InputProps>(
  ({
    variant = 'default',
    label,
    error,
    helperText,
    className = '',
    multiline = false,
    rows = 4,
    ...props
  }, ref) => {
    const baseClasses = 'block w-full px-3 py-2 border rounded-md shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2 transition-colors duration-200';

    const variantClasses = {
      default: 'border-gray-300 focus:border-blue-500 focus:ring-blue-500',
      error: 'border-red-300 focus:border-red-500 focus:ring-red-500',
      success: 'border-green-300 focus:border-green-500 focus:ring-green-500',
    };

    const finalVariant = error ? 'error' : variant;
    const finalClasses = `${baseClasses} ${variantClasses[finalVariant]} ${className} !text-primary`;

    return (
      <div className="w-full">
        {label && (
          <label className="block text-sm font-medium text-gray-700 mb-1">
            {label}
          </label>
        )}

        {multiline ? (
          <textarea
            ref={ref as any}
            rows={rows}
            className={finalClasses}
            {...(props as any)}
          />
        ) : (
          <input
            ref={ref as any}
            className={finalClasses}
            {...(props as any)}
          />
        )}

        {error && (
          <p className="mt-1 text-sm text-red-600">{error}</p>
        )}
        {helperText && !error && (
          <p className="mt-1 text-sm text-gray-500">{helperText}</p>
        )}
      </div>
    );
  }
);


Input.displayName = 'Input'; 