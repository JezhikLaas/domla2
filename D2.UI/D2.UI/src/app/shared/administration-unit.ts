import { Address } from './address';

export interface AdministrationUnit {
  id: string;
  userKey: string;
  title: string;
  address: Address;
}
