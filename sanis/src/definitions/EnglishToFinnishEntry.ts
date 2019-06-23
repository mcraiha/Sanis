import { IDictionaryDefinition } from '@/interfaces/IDictionaryDefinition';
import { Languages } from './LanguageEnums';

export class EnglishToFinnishEntry implements IDictionaryDefinition {
    public from: Languages = Languages.English;
    public to: Languages = Languages.Finnish;

    public fromUrl: string = 'https://en.wiktionary.org/wiki/';
    public toUrl: string = 'https://fi.wiktionary.org/wiki/';

    public entryName: string = '🇺🇸 -> 🇫🇮';
}
