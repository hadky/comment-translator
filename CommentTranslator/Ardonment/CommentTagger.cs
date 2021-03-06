﻿using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace CommentTranslator.Ardonment
{
    internal class CommentTagger : ITagger<CommentTranslateTag>
    {
        private readonly IClassificationTypeRegistryService _registry;
        private readonly ITagAggregator<IClassificationTag> _aggregator;

        public CommentTagger(IClassificationTypeRegistryService registry, ITagAggregator<IClassificationTag> tagAggregator)
        {
            this._aggregator = tagAggregator;
            this._registry = registry;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<CommentTranslateTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            //Get snapshot
            ITextSnapshot snapshot = spans[0].Snapshot;
            var contentType = snapshot.TextBuffer.ContentType;
            if (!contentType.IsOfType("code"))
                yield break;

            foreach (var tagSpan in _aggregator.GetTags(spans))
            {
                // find spans that the language service has already classified as comments ...
                string classificationName = tagSpan.Tag.ClassificationType.Classification;
                if (classificationName.IndexOf("comment", StringComparison.OrdinalIgnoreCase) <= 0)
                    continue;

                var nssc = tagSpan.Span.GetSpans(snapshot);
                if (nssc.Count > 0)
                {
                    var snapshotSpan = nssc[0];

                    string text = snapshotSpan.GetText();
                    if (String.IsNullOrWhiteSpace(text))
                        continue;

                    yield return new TagSpan<CommentTranslateTag>(snapshotSpan, new CommentTranslateTag(text, 200));
                }
            }
        }
    }
}
