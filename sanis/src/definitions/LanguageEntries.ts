import { IDictionaryDefinition } from '../interfaces/IDictionaryDefinition';
import { FinnishToEnglishEntry } from './FinnishToEnglishEntry';
import { EnglishToFinnishEntry } from './EnglishToFinnishEntry';

export abstract class LanguageEntries {
    public static entries: IDictionaryDefinition[] = [new FinnishToEnglishEntry(), new EnglishToFinnishEntry() ];
}
