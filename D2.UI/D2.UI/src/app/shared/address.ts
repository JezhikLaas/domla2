import { CountryInfo } from './country-info';

export interface Address {
  country: CountryInfo;
  city: string;
  street: string;
  number: string;
  postalCode: string;
}
