import {Entrance} from './../../../shared/entrance';

export interface AdministrationUnit {
  id: string;
  userKey: string;
  title: string;
  entrances: Entrance[];
}
