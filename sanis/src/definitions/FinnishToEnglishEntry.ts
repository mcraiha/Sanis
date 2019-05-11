import { IDictionaryDefinition } from '@/interfaces/IDictionaryDefinition';
import { Languages } from './LanguageEnums';

export class FinnishToEnglishEntry implements IDictionaryDefinition {
    public from: Languages = Languages.Finnish;
    public to: Languages = Languages.English;

    public fromUrl: string = 'https://fi.wiktionary.org/wiki/';
    public toUrl: string = 'https://en.wiktionary.org/wiki/';

    public entryName : string = '🇫🇮 -> 🇺🇸';
}