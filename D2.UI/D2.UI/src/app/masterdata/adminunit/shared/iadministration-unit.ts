import {Entrance} from './../../../shared/entrance';

export interface IAdministrationUnit {
  Id: string;
  UserKey: string;
  Title: string;
  Entrances: Entrance[];
  YearOfConstruction?: Date;
  Edit: Date;
}
