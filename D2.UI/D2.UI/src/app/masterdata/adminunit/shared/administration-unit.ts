import {Entrance} from '../../../shared/entrance';
import {IAdministrationUnit} from './iadministration-unit';


export class AdministrationUnit implements IAdministrationUnit {
  constructor(
    public Id: string,
    public UserKey: string,
    public Title: string,
    public Edit: Date,
    public Entrances?: Entrance[],
    public YearOfConstruction?: Date,
  ) { }
}
