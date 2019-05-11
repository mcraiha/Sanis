import { Languages } from '../definitions/LanguageEnums';

export interface IDictionaryDefinition {
    from: Languages;
    to: Languages;

    fromUrl: string;
    toUrl: string;

    entryName: string;
}