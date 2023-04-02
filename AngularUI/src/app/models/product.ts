
export interface Product {
    id: number;
    rating: number | null;
    defaultPrice: number | null;
    minPrice: number | null;
    maxPrice: number | null;
    shortDescription: string | null;
    description: string | null;
    productFullName: string | null;
    productExtendedFullName: string | null;
    productType: string | null;
    mainFileName: string | null;
    mainFilePath: string | null;
    creationDateTime: string | null;
    lastTimeEdited: string | null;
    parsedUrl: string | null;
}
