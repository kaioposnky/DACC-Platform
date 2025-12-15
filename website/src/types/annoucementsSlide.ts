type SlideType = 'code' | 'event' | 'default';

export interface SlideDetail {
    icon: 'calendar' | 'users' | 'clock' | 'location';
    text: string;
}
  
export interface Slide {
    content: string;
    id: number;
    title: string;
    type: SlideType;
    icon?: string;
    details?: SlideDetail[];
    primaryButtonText?: string;
    secondaryButtonText?: string;
    imageSrc?: string;
    imageAlt?: string;
    primaryButtonLink?: string;
    secondaryButtonLink?: string;
  }