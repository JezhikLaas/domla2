import { CountryInfo } from './country-info';

export class Address {
  public Country?: CountryInfo;
  public City: string;
  public Street: string;
  public Number: string;
  public PostalCode: string;
}
