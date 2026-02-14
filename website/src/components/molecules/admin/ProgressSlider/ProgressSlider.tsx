"use client";

import { Input } from "@/components";

interface ProgressSliderProps {
    label?: string;
    value: number;
    onChange: (value: number) => void;
    min?: number;
    max?: number;
    showInput?: boolean;
    className?: string;
}

export default function ProgressSlider({
    label = "Progresso",
    value,
    onChange,
    min = 0,
    max = 100,
    showInput = true,
    className = "",
}: ProgressSliderProps) {
    const handleSliderChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = parseInt(e.target.value) || 0;
        onChange(Math.min(Math.max(newValue, min), max));
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const newValue = parseInt(e.target.value) || 0;
        onChange(Math.min(Math.max(newValue, min), max));
    };

    return (
        <div className={`space-y-3 ${className}`}>
            <div className="flex justify-between items-end gap-4">
                <label className="text-xs font-bold text-gray-500 uppercase ml-1">
                    {label}
                </label>
                {showInput ? (
                    <div className="w-20">
                        <Input
                            type="number"
                            value={value}
                            onChange={handleInputChange}
                            min={min}
                            max={max}
                            className="text-right font-bold text-primary"
                        />
                    </div>
                ) : (
                    <span className="text-lg font-bold text-primary">{value}%</span>
                )}
            </div>

            <input
                type="range"
                min={min}
                max={max}
                value={value}
                onChange={handleSliderChange}
                className="w-full h-2 bg-gray-200 rounded-lg appearance-none cursor-pointer accent-primary"
            />

            <div className="flex justify-between text-[10px] text-gray-400 px-1">
                <span>{min}%</span>
                <span>{Math.floor((min + max) / 2)}%</span>
                <span>{max}%</span>
            </div>
        </div>
    );
}
