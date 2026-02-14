"use client";

import { Input } from "@/components";

interface DateTimeInputsProps {
    dateValue: string;
    timeValue: string;
    onDateChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    onTimeChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
    dateLabel?: string;
    timeLabel?: string;
    className?: string;
}

// Generate hours array (00-23)
const hours = Array.from({ length: 24 }, (_, i) => i.toString().padStart(2, "0"));
// Generate minutes array (00-59)
const minutes = Array.from({ length: 60 }, (_, i) => i.toString().padStart(2, "0"));

export default function DateTimeInputs({
    dateValue,
    timeValue,
    onDateChange,
    onTimeChange,
    dateLabel = "Data",
    timeLabel = "Horário",
    className = "",
}: DateTimeInputsProps) {
    // Parse current hour and minute from timeValue (HH:mm format)
    const [currentHour, currentMinute] = (timeValue || "00:00").split(":");

    const handleHourChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const newHour = e.target.value;
        const syntheticEvent = {
            target: { value: `${newHour}:${currentMinute || "00"}` }
        } as React.ChangeEvent<HTMLInputElement>;
        onTimeChange(syntheticEvent);
    };

    const handleMinuteChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const newMinute = e.target.value;
        const syntheticEvent = {
            target: { value: `${currentHour || "00"}:${newMinute}` }
        } as React.ChangeEvent<HTMLInputElement>;
        onTimeChange(syntheticEvent);
    };

    return (
        <div className={`space-y-4 ${className}`}>
            <Input
                label={dateLabel}
                type="date"
                value={dateValue.split("T")[0]}
                onChange={onDateChange}
            />
            <div>
                <label className="block text-xs font-bold text-gray-500 uppercase mb-1 ml-1">
                    {timeLabel}
                </label>
                <div className="flex items-center gap-2">
                    <select
                        value={currentHour || "00"}
                        onChange={handleHourChange}
                        className="flex-1 px-3 py-2.5 bg-white border border-gray-200 rounded-lg text-gray-700 text-sm font-medium focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
                    >
                        {hours.map((h) => (
                            <option key={h} value={h}>{h}</option>
                        ))}
                    </select>
                    <span className="text-gray-500 font-bold text-lg">:</span>
                    <select
                        value={currentMinute || "00"}
                        onChange={handleMinuteChange}
                        className="flex-1 px-3 py-2.5 bg-white border border-gray-200 rounded-lg text-gray-700 text-sm font-medium focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all"
                    >
                        {minutes.map((m) => (
                            <option key={m} value={m}>{m}</option>
                        ))}
                    </select>
                </div>
            </div>
        </div>
    );
}
