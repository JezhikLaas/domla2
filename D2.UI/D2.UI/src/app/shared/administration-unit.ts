import {Entrance} from './entrance';

export interface AdministrationUnit {
  id: string;
  userKey: string;
  title: string;
  entrances: Entrance[];
}
