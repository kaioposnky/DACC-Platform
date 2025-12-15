"use client";

import { Card } from '@/components/atoms';

interface CodeSnippetProps {
  className?: string;
}

export const CodeSnippet = ({ className = '' }: CodeSnippetProps) => {
  return (
      <Card className={`bg-gray-800 border-secondary p-6 shadow-2xl ${className}`}>
        <div className="font-mono text-lg space-y-2 gap-1 flex flex-col">
              <p className="text-secondary">
                const inovação = true;
              </p>
              <p className="text-secondary">
                let aprendizado = nuncaTermina;
              </p>
              <p className="text-secondary">
                {`function criarFuturo() {`}
              </p>
              <p className="text-secondary pl-4">
               return &quot;sucesso&quot;;
              </p>
              <p className="text-secondary">
                {`}`}
              </p>
            
        </div>
      </Card>
  );
}; 