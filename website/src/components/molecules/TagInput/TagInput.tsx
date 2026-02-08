"use client";

import React, { useState, KeyboardEvent } from "react";
import { XMarkIcon } from "@heroicons/react/20/solid";

interface TagInputProps {
  label?: string;
  tags: string[];
  onAddTag: (tag: string) => void;
  onRemoveTag: (tag: string) => void;
  placeholder?: string;
  maxTags?: number;
  disabled?: boolean;
}

export const TagInput = ({
  label,
  tags,
  onAddTag,
  onRemoveTag,
  placeholder = "Digite e pressione Enter...",
  maxTags,
  disabled = false,
}: TagInputProps) => {
  const [inputValue, setInputValue] = useState("");

  const handleKeyDown = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      e.preventDefault();
      const newTag = inputValue.trim();

      if (newTag && !tags.includes(newTag)) {
        if (maxTags && tags.length >= maxTags) return;
        onAddTag(newTag);
        setInputValue("");
      }
    }
  };

  return (
    <div className="w-full">
      {label && (
        <label className="block text-sm font-medium text-gray-700 mb-2">
          {label}
        </label>
      )}
      <div className="bg-white border border-gray-300 rounded-md px-3 py-2 shadow-sm focus-within:ring-2 focus-within:ring-blue-500 focus-within:border-blue-500 transition-all">
        <div className="flex flex-wrap gap-2">
          {tags.map((tag) => (
            <span
              key={tag}
              className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800"
            >
              {tag}
              <button
                type="button"
                onClick={() => onRemoveTag(tag)}
                disabled={disabled}
                className={`ml-1.5 inline-flex shrink-0 h-4 w-4 rounded-full text-blue-600 hover:bg-blue-200 hover:text-blue-900 focus:outline-none ${disabled ? "cursor-not-allowed opacity-50" : ""}`}
              >
                <XMarkIcon className="h-3 w-3" />
              </button>
            </span>
          ))}
          <input
            type="text"
            className={`flex-1 min-w-30 bg-transparent border-none focus:ring-0 p-0 text-sm text-gray-900 placeholder-gray-400 ${disabled ? "cursor-not-allowed opacity-50" : ""}`}
            placeholder={tags.length === 0 ? placeholder : ""}
            value={inputValue}
            onChange={(e) => setInputValue(e.target.value)}
            onKeyDown={handleKeyDown}
            disabled={disabled}
          />
        </div>
      </div>
      <p className="mt-1 text-xs text-gray-500">
        Pressione Enter para adicionar uma nova tag.
      </p>
    </div>
  );
};
