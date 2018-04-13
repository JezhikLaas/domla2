import {Entrance} from './../../../shared/entrance';

export interface IAdministrationUnit {
  Id: string;
  UserKey: string;
  Title: string;
  Version: number;
  Entrances?: Entrance[];
  YearOfConstruction?: Date;
  Edit: Date;
}
