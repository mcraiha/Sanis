import { IDictionaryDefinition } from '../interfaces/IDictionaryDefinition';
import { FinnishToEnglishEntry } from './FinnishToEnglishEntry';

export abstract class LanguageEntries {
    public static entries: IDictionaryDefinition[] = [new FinnishToEnglishEntry(), ];
}