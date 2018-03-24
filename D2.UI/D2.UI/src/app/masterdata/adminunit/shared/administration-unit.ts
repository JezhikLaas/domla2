import {Entrance} from '../../../shared/entrance';
import {IAdministrationUnit} from './iadministration-unit';


export class AdministrationUnit implements IAdministrationUnit {
  constructor(
    public id: string,
    public userKey: string,
    public title: string,
    entrances?: Entrance[]
  ) { }
}
