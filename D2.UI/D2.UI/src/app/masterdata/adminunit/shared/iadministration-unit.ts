import {Entrance} from './../../../shared/entrance';

export interface IAdministrationUnit {
  id: string;
  userKey: string;
  title: string;
  entrances?: Entrance[];
}
