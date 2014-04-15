using TraceLabSDK.Component.Config;

namespace AdvSoftEng.Components
{
    class CorpusImporterConfig
    {

        // doc1 - method identifiers
        private FilePath _ids;
        public FilePath Identifiers
        {
            get
            {
                return _ids;
            }
            set
            {
                _ids = value;
            }
        }

        // doc2 - BOW documents
        private FilePath _docs;
        public FilePath Documents
        {
            get
            {
                return _docs;
            }
            set
            {
                _docs = value;
            }
        }

    }
}
